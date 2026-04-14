THIS_SHOULD_BREAK;
<?php

use Illuminate\Http\Request;
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
});


/*Route::post('/register', function (Request $request) {

    $user = User::create([
        'email' => $request->email,
        'password' => Hash::make($request->password),
    ]);

    $token = $user->createToken('unity-token')->plainTextToken;

    return response()->json([
        'status' => 'success',
        'token' => $token,
        'user' => $user
    ]);
});*/


//dont use middleware keyword right now as that requires
//users to be authenticated, meaning they need CSRF token
//which they dont have since this is the first time they go on.
