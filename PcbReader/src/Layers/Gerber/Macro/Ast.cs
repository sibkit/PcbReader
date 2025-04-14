namespace PcbReader.Layers.Gerber.Macro;

public enum AstNodeType {
    Operation,
    Value,
    Parameter
}

public class AstNode {

    private readonly List<AstNode> _childNodes = [];
    private AstNode? _parent;

    public AstNode() { }
    private AstNodeType? Type { get; set; }
    public IToken? Token { get; set; }

    public AstNode[] ChildNodes => _childNodes.ToArray();

    public AstNode? Parent {
        get => _parent;
        set {
            if(_parent == value) return;
            _parent?._childNodes.Remove(this);
            _parent = value;
            _parent?._childNodes.Add(this);
        }
    }
}

public class AstBuilder {
    public AstNode Build(List<IToken> tokens) {

        // foreach (var t in tokens) {
        //     switch (t) {
        //         ParensOpenToken pon => {}
        //     }
        // }
        
        AstNode result = new AstNode();
        
        return result;
    }
}