
public class BehaviourTree{
    private Node root;
    public BehaviourTree(Node root){
        this.root = root;
    }

    public void Update(){
        if(root != null){
            root.Evaluate();
        }
    }
}
