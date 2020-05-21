# Symmetric and Asymmetric JWT Token encryption using .NET Core 3.x

## 1. Introduction

The idea behind this small repo is to give you sample code to show you how you can encode/decode/validate JWT tokens using Symmetric and Asymmetric algorithms. In addition, it will show you how you can integrate JWT bearer authentication into the ASP .NET Core pipeline to you can take advantage of the Authentication/Authorization capabilities that this framework offers.

## 2. Symmetric encryption

### What is this

Is basically a type of encryption where we only use a single key (a secret one) for both encrypt an decrypt electronic information. The parties communicating via symmetric encryption must exchange the key so it can be used in the decryption process.

## 2.1. How it works

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

The process to sign a a JWT token with an asymmetric key is similar. You also need to pass a key to the `JwtSecurityToken` constructor the only difference is that in this scenario you need to create a Rsa key. This is quick example:

```c#
// this would be a private key that you previously generated
// this keys are normally base64 encoded
var base64Key = @"
MIIEpgIBAAKCAQEAzLiIW0pWX0fgyHXhrfcbAzA2613o0kN2Lly3mcJC+roM8k+6
HqGfB+qvRRGitmR+K+Dsd0+qzqgrbyrEfScGLpdkFi0vGECHxkWbvNOSkd4YC4eb
ABkCW3kHJ9i3xUtryTq18Oxdk2RRnkDkl0xz35Lp4TVBApUVIjfDYa0XX9DVEzCF
5Mjn+SduJQgdpSafoQV5Osf5K5/U2dY8vyZ4YBov9tOnHlP7cHZz64yq/kd9AtpZ
4Xwqva2GnF8EwoxQOvv9xjK3Pvn5AxBIARuK4Sy7YJvB6Y/gsIUdCWPstfKakHRV
tYH8mRkZlr3GsLVTlr6pyMi9fOrd4Mj1Cq5crwIDAQABAoIBAQCadOe8KNv9y1dX
J0l8XaGtyCTHc/UDHw79b8q+YYjzbofyEiEUl1BLQs+2RtYC0tM2+96tLhhrDwqm
NxhEbLsHUAWdjb2oiJdoCoCz+NTu5SgzGr7hVf0nUqsJb0NHwd2y128KNOttFxVV
VxSg6nfKSSfYWLuR3TUyuevZuCSYjrZGGz1ISJdbLcmYpR34KzYvsq15qPJ1hlXH
RA69iVzws3wzDFaEVjBNAi3acNrW0cU1ZhEG5aMYHMgaizPagLERx+H9Ed8NJvpf
I0Ff9c29QN2ozGWwA0u0FCxdBJIMimtF/082OV7ntbgsC3wuRKCJyhSfDm7x9frp
rSa18SYRAoGBAPomKHi1TKzuLuqu3hsJEwZSHGMmD136EuRDgV69aer+dgm45h3u
1j2dEGpoIXzD0Zm5V+R4ugajXeUzwlE6upFKt82ncDUoMzy/lEN79Qx77U8RgQFh
T+V9/7qqny9N1U+ZlbXBnHvKC1YSkNI7ULtuYFOcnUHrsqYjkRw+dBt7AoGBANGC
XAq5OXxkiynexjVHcPSMprohEnbGBd2uesaeF6k8PNs1cc84rrn3tcuRchdGC1Zn
rTtYYlQpDozGcJ6AabGBic3T/9aui6IQx32aH3mLRGYjqoZmDbM8NrTsGxJ0Zy9l
2+Uv1kaetvNGBz3n7CTbCUKQrYHMfDvCMCa7btNdAoGBALYX7uAnwgAkhpyp3N67
VVEFlsqKGyei8fueNF+d7Kt2vsBHC6SAhw5k9l0vHilvdnW72QJk664HYEOH0Q1G
fidjx7b6CxF3CQ1QvIH/ixuiXtZu/ITfrMR3WyAsMiLCOuRt/fBIVzw5kNUAQNH5
AcTfDVnwKP2isPKF840m3WhfAoGBAJDqHWYhdlzPYt7kQ69Iqh3NR+nIxG+5Swdq
D2xgn7ckfmeV0RHngBwL9ghHo2Xrwh3S+qTo3qKd/31qKS3JXtIz6rCfKgBBnT3N
Y19CgvDcXsRTeTkbT/Cg/qooAhXCxDipuO8PJqSUVbFxx8KPL+zSQNZ1Ij58xZXE
HPA9h0vVAoGBAM/lM3D345WUFT+xv11dt4DE2ClPZs4URSrM2MsZPxtg9sdiRVsE
Xoi56r3wN9rXxVx9KvGS3GtN40i29V1ikDjtgRNOXmCv1RpG3I3xACLM2mgDDSIh
JFMSMl0pYyKgAZwcC1vy1T0ho4a+hul9j82occw/K0FQk3iSK+ocO3lW";

var rsa = RSA.Create();
// You may think that you also need to import the public key here
// in the end RSA algorithm uses a combination or public/private keys
// guess what, you already did it, because the "private" key contain both
rsa.ImportRSAPrivateKey(
    source: Convert.FromBase64String(base64Key),
    bytesRead: out _
);
```

Once we have our RSA key we can create our JWT token like this:

```c#
var signingCredentials = new SigningCredentials(
    // in this case we are using an instance of RsaSecurityKey
    key: new RsaSecurityKey(rsa),
    // and a different algorithm also
    algorithm: SecurityAlgorithms.RsaSsaPssSha256
);

var jwt = new JwtSecurityToken(
    ...
    signingCredentials: signingCredentials
    ...
);
```

An this is it, now we are creating JWT using asymmetric algorithm.

### 3.1. Generating RSA Keys

In order to make this sample work we would have to generate the keys that we are going to use. There are different ways to do this. In this case I provided two different paths. Either using the openssl tool if you have it installed or by using a console project provided as part of the repo that will output a pair of private/public rsa keys for you.

### 3.2. OpenSSL

Generating a private key:

```bash
openssl genrsa -out private.rsa
```

Extracting the public key:

```bash
openssl rsa -in private.rsa -pubout -out public.rsa
```

### 3.3. Utils Application

```bash
cd utils
dotnet run
```

## 4. Configuration

So where do we save all these keys while developing? Again, there different ways to accomplish this but I am going to show you how to user a new feature in .NET Core called user-secrets (please DO NOT use in production). The idea behind is to able to store the secrets that you use for development separated from your source code that way you do not have to check sensitive information into your source control system.

### 4.1. Initialize it

In order to initialize your user secrets store, you have to CD into the project that need those secrets (this is project specific) and run the following:

```bash
cd api
dotnet user-secrets init
```

This will add a specific secrets id to you your project file, example:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <!-- HERE -->
    <UserSecretsId>d27994da-a034-49e5-a4a3-3d6967699f84</UserSecretsId>
    <!--  -->
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.IdentityModel.Logging" Version="6.5.1" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\api.core.auth\api.core.auth.csproj" />
  </ItemGroup>
</Project>
```

At this point you are ready to add your own secrets. Even though it is not a requirement it is a good practice to have the same keys you are overwriting in you `appsettings.json`. For example let say you want to store a symmetric key that you are going to use when encrypting your JWT token. You add the following to your settings:

> appsettings.json

```json
{
    "Jwt": {
        "PrivateKey": "",
        "PublicKey": ""
    }
}
```

Then you would add those key paths to your `user-secrets` by doing the following:

```bash
dotnet user-secrets set "Jwt:PrivateKey" "private key goes here"
dotnet user-secrets set "Jwt:PublicKey" "public key goes here"
```

Now if you lunch your application the framework will "magically" overwrite those keys with the values stored in your `user-secrets`! (is not really magic but unfortunately explaining why here is out of te scope in this session)

## 5. JWT Token

At this point we have a general idea of how we can sign JWT tokens using symmetric and asymmetric algorithms but what is that??? yes, I know maybe some of you think I should have started with that but I think you will understand it better in this order.

### What is JSON Web Token

JSON Web Token (JWT) is an open standard (RFC 7519) that defines a compact and self-contained way for securely transmitting information between parties as a JSON object. This information can be verified and trusted because it is digitally signed. JWTs can be signed using a secret (with the HMAC algorithm) or a public/private key pair using RSA or ECDSA.

Although JWTs can be encrypted to also provide secrecy between parties, we will focus on signed tokens. Signed tokens can verify the integrity of the claims contained within it, while encrypted tokens hide those claims from other parties. When tokens are signed using public/private key pairs, the signature also certifies that only the party holding the private key is the one that signed it.