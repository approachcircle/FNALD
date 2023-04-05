using System;

public class Cameron : Monster
{
    new public string Name { get; set; } = "Cameron";
    new public int Hostility { get; set; } = 0;
    public override bool Roll()
    {
        return false;
    }

    public override void SetDestination()
    {
        throw new NotImplementedException();
    }
}