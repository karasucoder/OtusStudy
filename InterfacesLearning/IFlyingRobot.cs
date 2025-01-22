namespace InterfacesLearning;

public interface IFlyingRobot : IRobot
{
    string GetRobotType()
    {
        return "I am a flying robot.";
    }
}
