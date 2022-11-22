public class Rect {
    public int xMax;
    public int yMax;
    public int xMin;
    public int yMin;
    public Rect(int xMin, int xMax, int yMin, int yMax){
        this.xMax=xMax;
        this.xMin=xMin;
        this.yMax=yMax;
        this.yMin=yMin;
    }
    public String toString(){
        return "x:"+xMin+"-"+xMax+ " y:"+yMin+"-"+yMax; 
    }   
}
