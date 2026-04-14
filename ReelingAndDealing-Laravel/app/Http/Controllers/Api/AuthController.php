<?php
namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\User;
use Illuminate\Support\Facades\Hash;
use Illuminate\Validation\ValidationException;

class AuthController extends Controller
{
    public function login(Request $request)
{
    $request->validate([
        'username' => 'required|string',
        'password' => 'required|string',
    ]);

    $user = User::where('username', $request->username)->first();

    // AUTO REGISTER if not exists
    if (!$user) {
        $user = User::create([
            'username' => $request->username,
            'name' => $request->username, // optional display name
            'password' => Hash::make($request->password),
        ]);
    }

    // CHECK PASSWORD
    if (!Hash::check($request->password, $user->password)) {
        return response()->json([
            'message' => 'Invalid login'
        ], 401);
    }

    $token = $user->createToken('unity-game-token')->plainTextToken;

    return response()->json([
        'token' => $token,
        'user' => $user
    ]);
}