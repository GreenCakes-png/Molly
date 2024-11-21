using Neo.SmartContract.Framework;

namespace Neo.SmartContract.Template
{
    public class PlayerTokenState : Nep11TokenState
    {
        public string Position { get; set; }
        public string Image { get; set; }
        public string League { get; set; }
        public string Team { get; set; }
    }
}
