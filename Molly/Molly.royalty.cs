using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Attributes;
using Neo.SmartContract.Framework.Interfaces;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;

using System;
using System.ComponentModel;
using System.Numerics;


namespace Neo.SmartContract.Template
{
    public partial class Molly
    {
        [Safe]
        public static Map<string, object>[] RoyaltyInfo(ByteString tokenId, UInt160 royaltyToken, BigInteger salePrice)
        {
            ExecutionEngine.Assert(OwnerOf(tokenId) != null, "This TokenId doesn't exist!");
            var royaltyInfo = new Map<string, object>();
            royaltyInfo["royaltyRecipient"] = GetMinter();
            royaltyInfo["royaltyAmount"] = salePrice / 1000;
            return new[] { royaltyInfo };
        }
    }
}