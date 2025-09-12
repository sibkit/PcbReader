namespace SibtronicPcbHandler;

public enum MaterialType {
    FR4,
    Aluminium
}

public class MaterialLayer: IBoardLayer {
    public MaterialLayer(MaterialType materialType, double width) {
        MaterialType = materialType;
        Width = width;
    }
    public MaterialType MaterialType { get; }
    public double Width { get; }
}