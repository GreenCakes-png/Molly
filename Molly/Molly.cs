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
    [DisplayName(nameof(Molly))]
    [ContractAuthor("<Your Name Or Company Here>", "<Your Public Email Here>")]
    [ContractDescription( "<Description Here>")]
    [ContractVersion("<Version String Here>")]
    [ContractPermission(Permission.Any, Method.Any)]
    [SupportedStandards(NepStandard.Nep24)]
    public partial class Molly : Nep11Token<PlayerTokenState>, INep24
    {
        public new static bool Transfer(UInt160 to, ByteString tokenId, object data)
        {
            ExecutionEngine.Abort("Not vlaid");
            var tokenMap = new StorageMap(Prefix_Token);
            var token = (PlayerTokenState)StdLib.Deserialize(tokenMap[tokenId]);
            var league = GetLeague(token.League);

            if(Runtime.Time > league.End)
            {
                return Nep11Token<PlayerTokenState>.Transfer(to, tokenId, data);
            }

            var sender = (UInt160)Runtime.Transaction.Sender;
            if(sender.Equals(GetCoach()) || to.Equals(GetCoach()))
                return false;

            UInt160 from = token.Owner;
            if (from != to)
            {
                token.Owner = to;
                tokenMap[tokenId] = StdLib.Serialize(token);
                UpdateBalance(from, tokenId, -1);
                UpdateBalance(to, tokenId, +1);
            }
            PostTransfer(from, to, tokenId, data);
            return true;
        }
    }
}
