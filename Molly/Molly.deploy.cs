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
    public partial class Molly : Nep11Token<PlayerTokenState>, INep24
    {
        // TODO: Replace "EXAMPLE" with a short name all UPPERCASE 3-8 characters
        public override string Symbol { [Safe] get => "LEAGUE"; }

        // This will be executed during deploy
        public static void _deploy(object data, bool update)
        {
            if (update)
            {
                // This will be executed during update
                return;
            }

            // Init method, you must deploy the contract with the owner as an argument, or it will take the sender
            if (data is null) data = Runtime.Transaction.Sender;

            UInt160 initialOwner = (UInt160)data;

            ExecutionEngine.Assert(initialOwner.IsValid && !initialOwner.IsZero, "owner must exists");

            Storage.Put(new[] { Prefix_Owner }, initialOwner);
            OnSetOwner(initialOwner);
            Storage.Put(Storage.CurrentContext, "Hello", "World");
        }

        public static void Update(ByteString nefFile, string manifest)
        {
            if (IsOwner() == false)
                throw new InvalidOperationException("No authorization.");
            ContractManagement.Update(nefFile, manifest);
        }
    }
}