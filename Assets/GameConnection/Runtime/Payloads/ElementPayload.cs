using Common.Scripts.PayloadTypes;

public class ElementPayload : ScenePayloadBase
{
    public readonly Element Element;

    public ElementPayload(Element element)
    {
        Element = element;
    }
}