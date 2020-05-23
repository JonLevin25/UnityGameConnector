namespace GameConnection.Payloads.TestPayloads
{
    public class ElementPayload : ScenePayloadBase
    {
        public readonly Element Element;

        public ElementPayload(Element element)
        {
            Element = element;
        }

        public override string ToString()
        {
            return Element.ToString();
        }
    }
}
