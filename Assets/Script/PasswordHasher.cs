using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public sealed class PasswordHasher
{
    public byte Version => 1;
    public int Pbkdf2IterCount { get; } = 50000;
    public int Pbkdf2SubkeyLength { get; } = 256 / 8; // 256 bits
    public int SaltSize { get; } = 128 / 8; // 128 bits
    public HashAlgorithmName HashAlgorithmName { get; } = HashAlgorithmName.SHA256;

    public string HashPassword(string password)
    {
        if (password == null)
            throw new ArgumentNullException(nameof(password));

        byte[] salt;
        byte[] bytes;
        using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltSize, Pbkdf2IterCount))
        {
            salt = rfc2898DeriveBytes.Salt;
            bytes = rfc2898DeriveBytes.GetBytes(Pbkdf2SubkeyLength);
        }

        var inArray = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
        inArray[0] = Version;
        Buffer.BlockCopy(salt, 0, inArray, 1, SaltSize);
        Buffer.BlockCopy(bytes, 0, inArray, 1 + SaltSize, Pbkdf2SubkeyLength);

        return Convert.ToBase64String(inArray);
    }

    public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string password)
    {
        if (password == null)
            throw new ArgumentNullException(nameof(password));

        if (hashedPassword == null)
            return PasswordVerificationResult.Failed;

        byte[] numArray = Convert.FromBase64String(hashedPassword);
        if (numArray.Length < 1)
            return PasswordVerificationResult.Failed;

        byte version = numArray[0];
        if (version > Version)
            return PasswordVerificationResult.Failed;

        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(numArray, 1, salt, 0, SaltSize);
        byte[] a = new byte[Pbkdf2SubkeyLength];
        Buffer.BlockCopy(numArray, 1 + SaltSize, a, 0, Pbkdf2SubkeyLength);
        byte[] bytes;
        using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, Pbkdf2IterCount))
        {
            bytes = rfc2898DeriveBytes.GetBytes(Pbkdf2SubkeyLength);
        }

        if (FixedTimeEquals(a, bytes))
            return PasswordVerificationResult.Success;

        return PasswordVerificationResult.Failed;
    }

    
    // In .NET Core 2.1, you can use CryptographicOperations.FixedTimeEquals
    // https://github.com/dotnet/runtime/blob/419e949d258ecee4c40a460fb09c66d974229623/src/libraries/System.Security.Cryptography.Primitives/src/System/Security/Cryptography/CryptographicOperations.cs#L32
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool FixedTimeEquals(byte[] left, byte[] right)
    {
        // NoOptimization because we want this method to be exactly as non-short-circuiting as written.
        // NoInlining because the NoOptimization would get lost if the method got inlined.
        if (left.Length != right.Length)
        {
            return false;
        }

        int length = left.Length;
        int accum = 0;

        for (int i = 0; i < length; i++)
        {
            accum |= left[i] - right[i];
        }

        return accum == 0;
    }
}

public enum PasswordVerificationResult
{
    Failed,
    Success,
    SuccessRehashNeeded,
}
