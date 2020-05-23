namespace GameConnection.Payloads
{
    public class NamePayload : ScenePayloadBase
    {
        public readonly string PlayerName;
        public readonly string NemesisName;

        public NamePayload(string playerName, string nemesisName)
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