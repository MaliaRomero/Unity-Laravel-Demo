<?php

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;
use Illuminate\Support\Facades\URL;
use App\Http\Controllers\Api\AuthController;
use Illuminate\Auth\Events\Verified;
use App\Models\User;

Route::get('/email/verify/{id}/{hash}', function (Request $request, $id, $hash) {

    if (! URL::hasValidSignature($request)) {
        return response()->json(['message' => 'Invalid or expired link'], 403);
    }

    $user = User::find($id);

    if (! $user) {
        return response()->json(['message' => 'User not found'], 404);
    }

    if (! hash_equals(sha1($user->getEmailForVerification()), $hash)) {
        return response()->json(['message' => 'Invalid verification hash'], 403);
    }

    if ($user->hasVerifiedEmail()) {
        return response()->json(['message' => 'Email already verified']);
    }

    $user->markEmailAsVerified();

    event(new Verified($user));

    return response()->json(['message' => 'Email verified successfully']);

})->middleware(['signed'])->name('verification.verify');



Route::post('/login', [AuthController::class, 'login']);

Route::post('/register', [AuthController::class, 'register']);

Route::get('/leaderboard', [AuthController::class, 'leaderboard']);

Route::middleware('auth:sanctum')->group(function () {

Route::post('/update-score', [AuthController::class, 'saveScore']);

});