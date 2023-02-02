using System;

namespace GitSave.Models;

public class Commit
{
    public string UUID { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public DateTime Created { get; set; }
    public string Comment { get; set; }
}