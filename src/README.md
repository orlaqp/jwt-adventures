# Symmetric and Asymmetric JWT Token encryption using .NET Core 3.x

## 1. Introduction

The idea behind this small repo is to give you sample code to show you how you can encode/decode/validate JWT tokens using Symmetric and Asymmetric algorithms. In addition, it will show you how you can integrate JWT bearer authentication into the ASP .NET Core pipeline to you can take advantage of the Authentication/Authorization capabilities that this framework offers.

## 2. Symmetric encryption

#### What is this?

Is basically a type of encryption where we only use a single key (a secret one) for both encrypt an decrypt electronic information. The parties communicating via symmetric encryption must exchange the key so it can be used in the decryption process.

## 2.1. How it works?

Using the framework classes is very easy to implement this algorithm. For example:

```c#
// this is how you create an instance of a symmetric security key
// notice the key needs to be a byte array
var keyBytes = Encoding.UTF8.GetBytes("some key here");
var key = new SymmetricSecurityKey(keyBytes);
```

With this key we can sign JWT Tokens like this:

```c#
var signingCredentials = new SigningCredentials(
    key: new SymmetricSecurityKey(key),
    algorithm: SecurityAlgorithms.HmacSha256
);

var jwt = new JwtSecurityToken(
    ...
    signingCredentials: signingCredentials
    ...
);
```
At a very high level this is how you should create a JWT token using a symmetric key. 

## 3. Asymmetric encryption

The process to sign a a JWT token with symmetric key is similar. You also need to pass a key to the `JwtSecurityToken` constructor the only difference is that in this scenario you need to create a Rsa key. This is quick example:

```c#

```

### 3.1. Generating RSA Keys

In order

### 3.2. OpenSSL

Generating a private key:

```
openssl genrsa -out private.rsa 
```

Extracting the public key:

```
openssl rsa -in private.rsa -pubout -out public.rsa
```

## 3.2. Utils Application

```
cd utils
dotnet run
```

## 4. JWT Token