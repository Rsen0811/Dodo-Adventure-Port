import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.Reader;
import java.util.*;

public class RectangleHealing {
    //available optimization space with combining rectangles one on top of another
    //use the fact that if three of the numbers in the def are alike then can combine 
    //the one number that doesnt matchup tells you what changes
    public static int[][] map= new int[22][32];
    public static boolean[][] visited=new boolean[map.length][map[0].length];
    public static ArrayList<Rect> rects= new ArrayList<Rect>();
    
    public static void main(String[] args) throws FileNotFoundException, IOException, NoSuchElementException {
        String s="25";
        BufferedReader br=new BufferedReader(new FileReader(s+".txt"));
        for(int i=0;i<map.length;i++){
            StringTokenizer st=new StringTokenizer(br.readLine());
            int j=0;
            while(st.hasMoreTokens()){
                map[i][j]=Integer.parseInt(st.nextToken());
                j++;
            }    
        }
        //read the text file into the 
        for(int y=0;y<map.length;y++){
            for(int x=0;x<map[y].length;x++){
                if(!visited[y][x]&& map[y][x]==1){
                    //call recursive method here
                    StartRect(x,y);
                    System.out.println(rects.get(rects.size()-1).toString());
                }
            }
        }
        br.close();
        File newFile = new File(s+"c"+".txt");
        newFile.createNewFile();
        FileWriter fw=  new FileWriter(s+"c"+".txt");
        
        for (Rect rect : rects) {
            fw.write(rect.toString()+"\n");
        }
        fw.close();

        //StartRect(0,0);
        //System.out.println(rects.get(rects.size()-1).toString());
        //System.out.println(Arrays.deepToString(visited));
    }
    
    public static void StartRect(int x,int y){
        ExpandRect(x, y,-9,new Rect(x,x+1,y,y+1));
    }
    public static void ExpandRect(int x, int y,int currDir,Rect currRect){
        visited[y][x]=true;
        if(currDir==-1){
                //if negative 1 is a dir you can move then recurse
                //else add to the rect thingy
                try{
                   if(map[y][x-1]==1 && !visited[y][x-1]){
                        ExpandRect(x-1, y, currDir, new Rect(x, currRect.xMax, currRect.yMin, currRect.yMax));
                    }else{
                        if(!visited[y][x-1]){
                            rects.add(new Rect(x, currRect.xMax, currRect.yMin, currRect.yMax));
                        }
                    }
                }catch(Exception e){
                    rects.add(new Rect(x, currRect.xMax, currRect.yMin, currRect.yMax));
                }
        }
        else if(currDir==1){
            //if positive 1 is a dir you can move then recurse
            //else add to the rect thingy
            //System.out.println(currRect.toString());
            try{
                if(map[y][x+1]==1 && !visited[y][x+1]){
                     ExpandRect(x+1, y, currDir, new Rect(currRect.xMin, x+1, currRect.yMin, currRect.yMax));
                }else{
                     rects.add(new Rect(currRect.xMin, x+1, currRect.yMin, currRect.yMax));
                     }
             }catch(Exception e){
                rects.add(new Rect(currRect.xMin, x+1, currRect.yMin, currRect.yMax));
             }    
        }else if(currDir==-2){
            try{
                if(map[y+1][x]==1 && !visited[y+1][x]){
                     ExpandRect(x, y+1, currDir, new Rect(currRect.xMin, currRect.xMax, currRect.yMin, y+1));
                }else{
                     rects.add(new Rect(currRect.xMin, currRect.xMax, currRect.yMin, y+1));
                     }
             }catch(Exception e){
                 rects.add(new Rect(currRect.xMin, currRect.xMax, currRect.yMin, y+1));
             }    
        }else if(currDir==2){
            try{
                if(map[y-1][x]==1 && !visited[y-1][x]){
                     ExpandRect(x, y-1, currDir, new Rect(currRect.xMin, currRect.xMax, y, currRect.yMax));
                }else{
                     rects.add(new Rect(currRect.xMin, currRect.xMax, y, currRect.yMax));
                     }
             }catch(Exception e){
                 rects.add(new Rect(currRect.xMin, currRect.xMax, y, currRect.yMax));
             }    
        }else{
            ExpandRect(x,y,BestDir(x, y),currRect);
        }
    }
    //if 1 then move right
    //if -1 then move left
    //if 2 then move up 
    //if -2 then move down
    public static int BestDir(int x,int y){
        
        if(howFarX(x, y, 1)>howFarX(x, y, -1)){
            if(howFarY(x,y,1)>howFarY(x, y, -1)){
                if(howFarX(x, y, 1)>howFarY(x, y, 1)){
                    return 1;
                }
                else{
                    return -2;
                }
            }else{
                if(howFarX(x, y, 1)>howFarY(x, y, -1)){
                    return 1;
                }
                else{
                    return 2;
                }
            }
        }else{
            if(howFarY(x,y,1)>howFarY(x, y, -1)){
                if(howFarX(x, y, -1)>howFarY(x, y, 1)){
                    return -1;
                }
                else{
                    return -2;
                }
            }else{
                if(howFarX(x, y, -1)>howFarY(x, y, -1)){
                    return 1;
                }
                else{
                    return 2;
                }
            }
        }
    }
    public static int howFarX(int x,int y, int currDir){
        try{
            if (currDir>0){
                if(map[y][x+1]==1 && !visited[y][x+1]){
                    return 1+howFarX(x+1, y, currDir);
                }
            }else{
                if(map[y][x-1]==1 && !visited[y][x-1]){
                    return 1+howFarX(x-1, y, currDir);
                }
            }
        }catch(Exception e){
            return 0;
        }
        return 0;
    }
    public static int howFarY(int x,int y, int currDir){
        try{
            if (currDir>0){
                if(map[y+1][x]==1 && !visited[y+1][x]){
                    return 1+howFarY(x, y+1, currDir);
                }
            }else{
                if(map[y-1][x]==1 && !visited[y-1][x]){
                    return 1+howFarY(x, y-1, currDir);
                }
            }
        }catch(Exception e){
            return 0;
        }
        return 0;
    }
}
