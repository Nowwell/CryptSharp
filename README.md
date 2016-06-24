# CryptSharp
Classical ciphers for C#

Among the implemented Ciphers are FourSquare, Hill, Vigenere and many others


Usage
-----
General usage is as follows:
```
Cipher variableName = new Cipher(alphabetCharArray);
variableName.Key = "CRYPTOTRICK";

string cipherText = variableName.Encrypt(clearText);
string clearText = variableName.Decrypt(cipherText);
```
