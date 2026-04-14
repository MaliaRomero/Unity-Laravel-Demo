<?php

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\Api\AuthController;

Route::post('/login', [AuthController::class, 'login']);
Route::get('/leaderboard', [AuthController::class, 'leaderboard']);

/*Route::middleware('auth:sanctum')->group(function () {
    Route::post('/update-score', [AuthController::class, 'saveScore']);
    Route::post('/update-sprite', [AuthController::class, 'saveSprite']);
});*/

/*use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;
use Illuminate\Support\Facades\Auth;
use App\Models\User;
use Illuminate\Support\Facades\Hash;


Route::post('/login', function (Request $request) {
    $credentials = $request->only('email', 'password');

    if (Auth::attempt($credentials)) {
        $user = Auth::user();

        $token = $user->createToken('unity-token')->plainTextToken;

        return response()->json([
            'status' => 'success',
            'token' => $token,
            'user' => $user
        ]);
    }

    return response()->json([
        'status' => 'error',
        'message' => 'Invalid credentials'
    ], 401);
});

Route::post('/register', function (Request $request) {
    \Log::info('REGISTER ROUTE HIT', [
        'email' => $request->email,
        'route_version' => 'WITH_NAME_FIELD'
    ]);

    $user = User::create([
        'name' => 'Player',
        'email' => $request->email,
        'password' => Hash::make($request->password),
    ]);

    return response()->json(['success' => true, 'user' => $user]);
}); */