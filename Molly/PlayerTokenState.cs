using Neo.SmartContract.Framework;

namespace Neo.SmartContract.Template
{
    public class PlayerTokenState : Nep11TokenState
    {
        public string Position { get; set; }
        public string League { get; set; }
    }
}
