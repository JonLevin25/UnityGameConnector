namespace GameConnection.Payloads.TestPayloads
{
    public class NamesPayload : ScenePayloadBase
    {
        public readonly string PlayerName;
        public readonly string NemesisName;

        public NamesPayload(string playerName, string nemesisName)
        {
            PlayerName = playerName;
            NemesisName = nemesisName;
        }
        
        public override string ToString()
        {
            return $"PlayerName: {PlayerName}. NemesisName: {NemesisName}";
        }
    }
}