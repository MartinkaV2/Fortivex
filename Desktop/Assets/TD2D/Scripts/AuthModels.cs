using System;

[Serializable]
public class RegisterRequest
{
    public string userName;      // Swaggeren: "userName"
    public string passwordHash;  // Swaggeren: "passwordHash"
    public string email;         // Swaggeren: "email"
    public string role;          // Swaggeren: "role" (ezt is k�ri a regisztr�ci�!)
}

[Serializable]
public class LoginRequest
{
    public string userName;      // Swaggeren: "userName" (NEM email!)
    public string passwordHash;  // Swaggeren: "passwordHash"
}

[Serializable]
public class AuthResponse
{
    // Ezt a v�laszt a backendt�l kapjuk, lehet, hogy csak egy sima string �zenet j�n,
    // de ha JSON j�n, akkor ide kell illeszteni a mez�it.
    // Egyel�re hagyjuk �gy, a logokb�l majd l�tjuk, mit k�ld vissza pontosan.
    public string token;
    public int accountId;
    public string message;
}