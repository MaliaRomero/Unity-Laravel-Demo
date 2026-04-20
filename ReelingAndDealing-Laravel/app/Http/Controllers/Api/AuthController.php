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

        if (!$user) {
            $user = User::create([
                'username' => $request->username,
                'name' => $request->username,
                'password' => Hash::make($request->password),
            ]);
        }

        if (!Hash::check($request->password, $user->password)) {
            return response()->json(['message' => 'Invalid login'], 401);
        }

        $token = $user->createToken('unity-game-token')->plainTextToken;

        return response()->json([
            'token' => $token,
            'user' => $user
        ]);
    }

    public function leaderboard()
    {
        $users = User::orderBy('score', 'desc')
            ->limit(10)
            ->get(['username', 'score']);

        return response()->json([
            'items' => $users
        ]);
    }

}