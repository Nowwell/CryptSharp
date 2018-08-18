using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptSharp.Hashes;

namespace CryptSharp.Test
{
    [TestClass]
    public class HashTest
    {
        [TestMethod]
        public void Hash_MD5()
        {
            MD5 md = new MD5();

            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", md.Hash(""));
            Assert.AreEqual("0cc175b9c0f1b6a831c399e269772661", md.Hash("a"));
            Assert.AreEqual("900150983cd24fb0d6963f7d28e17f72", md.Hash("abc"));
            Assert.AreEqual("f96b697d7cb7938d525a2f31aaf161d0", md.Hash("message digest"));
            Assert.AreEqual("c3fcd3d76192e4007dfb496cca67e13b", md.Hash("abcdefghijklmnopqrstuvwxyz"));
            Assert.AreEqual("d174ab98d277d9f5a5611c2c9f419d9f", md.Hash("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"));
            Assert.AreEqual("57edf4a22be3c955ac49da2e2107b67a", md.Hash("12345678901234567890123456789012345678901234567890123456789012345678901234567890"));

        }
    }
}
