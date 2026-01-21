using System;

[Serializable]
public class RegisterRequest
{
    public string userName;      // Swaggeren: "userName"
    public string passwordHash;  // Swaggeren: "passwordHash"
    public string email;         // Swaggeren: "email"
    public string role;          // Swaggeren: "role" (ezt is kéri a regisztráció!)
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
    // Ezt a választ a backendtõl kapjuk, lehet, hogy csak egy sima string üzenet jön,
    // de ha JSON jön, akkor ide kell illeszteni a mezõit.
    // Egyelõre hagyjuk így, a logokból majd látjuk, mit küld vissza pontosan.
    public string token;
    public string message;
}