# CryptSharp
Some Classical and modern ciphers for C#

Among the implemented Ciphers are FourSquare, Hill, Vigenere and many others

There are 3 (incomplete) projects within this solution.
- CryptSharp - a dll that contains some classical ciphers
- CryptSharp.Test- a project to test the ciphers
- Cryptalize - a desktop app to analyze classical ciphers and hopefully be a game to generate some cipher text for fun to break

Note: This is not intended to be a professionally used tool.

Utility
-------
In CryptSharp there is a class called Utility.  Within this class are several functions to help with generic tasks associated with ciphers and cryptanalysis.

Usage
-----
General usage is as follows:
```
Cipher variableName = new Cipher(alphabetCharArray);
variableName.Key = "CRYPTOTRICK";

string cipherText = variableName.Encrypt(clearText);
string clearText = variableName.Decrypt(cipherText);
```

Not all ciphers have a "Key", some have arrays and other such things.  Look in the tests to see usage.

Ciphers Included
----------------

Classical (monograph)
- ADFGVX
- ADFGX
- Affine
- Amsco
- Atbash
- Baconain
- Baaufort
- Bifid
- Columnar
- FourSquare
- Hill
- Homophonic
- Playfair
- Polybius
- Porta
- RailFence
- Rotation
- Skip
- Substitution
- Trifid
- Xor

Classic (multigraph)
- Affine
- Atbash
- Baconian
- Polybius
- RailFence
- Rotation
- Substitution

Modern
- AES
- Rijndael
- DES
- Triple DES
- RSA
- Stream
- Trivium

There is also an implementation of a LinearFeedbackShiftREgister that allows you to Fibonacci Shift, Galois Shift, or a general Polynomial Shift.

I have included a version of Mersenne Twiser as a random number generator, but I typically use the cryptographically secure random number generator included with .NET
