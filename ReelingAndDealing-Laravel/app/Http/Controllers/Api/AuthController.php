<?php
namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\User;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\URL;
use Illuminate\Auth\Events\Verified;
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
            return response()->json([
                'message' => 'User not found'
            ], 404);
        }

        if (!Hash::check($request->password, $user->password)) {
            return response()->json([
                'message' => 'Invalid password'
            ], 401);
        }

        if (!$user->hasVerifiedEmail()) {
            return response()->json([
                'message' => 'Email not verified'
            ], 403);
        }        

        $token = $user->createToken('unity-game-token')->plainTextToken;

        return response()->json([
            'token' => $token,
            'user' => $user
        ]);
    }

    public function register(Request $request)
    {
        $request->validate([
            'username' => 'required|string|unique:users',
            'email' => 'required|email|unique:users',
            'password' => 'required|string|min:6',
        ]);
    
        $user = User::create([
            'username' => $request->username,
            'name' => $request->username,
            'email' => $request->email,
            'password' => Hash::make($request->password),
            'score' => 0,
        ]);
    
        $user->sendEmailVerificationNotification();
    
        return response()->json([
            'message' => 'Account created successfully'
        ], 201);
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

    public function saveScore(Request $request)
    {
        $user = $request->user();
        $user->score = max($user->score, $request->score); //saves highest score
        $user->save();

        return response()->json(['status' => 'Success', 'new_score' => $user->score]);
    }
}
