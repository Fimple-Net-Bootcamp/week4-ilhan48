﻿namespace PetManagement.Contracts;

public class CreateSocialInteractionRequest
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
