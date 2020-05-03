public class InputState
{
    private static string state = "normal";
    
    public string getState(){
        return state;
    }

    public void setState(string stateName){
        state = stateName;
    }
}
