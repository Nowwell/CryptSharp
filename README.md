# CryptSharp
Some classical and modern ciphers for C#

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

Classical (multigraph)
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

There is also an implementation of a LinearFeedbackShiftRegister that allows you to Fibonacci Shift, Galois Shift, or a general Polynomial Shift.

I have included a version of Mersenne Twiser as a random number generator, but I typically use the cryptographically secure random number generator included with .NET

Generic Ciphers
---------------
I have attempted to create some generic ciphers:
- Classical
- Block
- Stream

These generic ciphers contain delegates where you can plug in functions and keys of any type you want and not have to worry about the rote setup of each cipher.  Instead you can focus on the algorithm of the cipher itself.

The names of the generic functions are takan from Shannon saying that cryptography is about diffusion and confusion.

*Generic Classical*
You can have any of 3 types of keys and associated initialization vectors (keep in mind that even though it's called a key, it can be a state, table, etc.).  There are four functions you may set.
- DiffuseFunction
- InverseDiffuse
- ConfuseFunction
- InverConfuse

And then there's an alphabet to supply it.

*Generic Stream*
The generic stream cipher has two functions to supply it.
- KeystreamFunction
- ResetStateFunction

Keystream is to generate your stream bits used for encryption and decryption, and Reset state is so you can encrypt something, reset the state, and then decrypt that same thing.

The state, key, and initialization vector are all available for use as well.

*Generic Block*
You can have up to 2 keys and an associated initialization vectors, and there are four functions you may set.  Included with this cipher is also a block size that is defaulted to 128 bits.
- DiffuseFunction
- InverseDiffuse
- ConfuseFunction
- InverConfuse

To see how to use each of these generic ciphers, see the Generic Cipher Tests in the test project.
