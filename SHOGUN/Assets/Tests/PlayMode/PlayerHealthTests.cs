using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

public class PlayerHealthTests
{
    
    [UnityTest]
    public IEnumerator TakeDamageTest()
    {
        var player = new GameObject().AddComponent<PlayerHealth>();
        player.TakeDamage(3);
        Assert.AreEqual(97, player.GetPlayerHealth());
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator ShieldTest()
    {
        var player = new GameObject().AddComponent<PlayerHealth>();
        player.GiveShield(5);
        Assert.AreEqual(5, player.GetPlayerShield());
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator TakeDamageWithShieldTest()
    {
        var player = new GameObject().AddComponent<PlayerHealth>();
        player.GiveShield(5);
        player.TakeDamage(10);
        Assert.AreEqual(95, player.GetPlayerHealth());
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator HealTest()
    {
        var player = new GameObject().AddComponent<PlayerHealth>();
        player.TakeDamage(10);
        player.HealPlayer(2);
        Assert.AreEqual(92, player.GetPlayerHealth());
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator BleedStacksAfterDamageTest()
    {
        var player = new GameObject().AddComponent<PlayerHealth>();
        player.AddBleedStacks(10);
        player.TakeBleedDamage();
        player.TakeBleedDamage();
        Assert.AreEqual(8, player.GetPlayerBleedStacks());
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator BleedStacksDamageTest()
    {
        var player = new GameObject().AddComponent<PlayerHealth>();
        player.AddBleedStacks(10);
        player.TakeBleedDamage();
        player.TakeBleedDamage();
        Assert.AreEqual(81, player.GetPlayerHealth());
        yield return null;
    }
}