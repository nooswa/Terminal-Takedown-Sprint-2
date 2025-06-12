using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class BossHealthTests
{
    private GameObject bossGameObject;
    private BossHealth bossHealth;
    private GameObject healthBarUI;
    private Image healthBarFill;

    [SetUp]
    public void SetUp()
    {
        // Create boss GameObject with BossHealth component
        bossGameObject = new GameObject("Boss");
        bossHealth = bossGameObject.AddComponent<BossHealth>();

        // Create health bar UI GameObject
        healthBarUI = new GameObject("HealthBarUI");

        // Create health bar fill Image
        var healthBarFillObject = new GameObject("HealthBarFill");
        healthBarFill = healthBarFillObject.AddComponent<Image>();

        // Configure BossHealth
        bossHealth.maxHealth = 5;
        bossHealth.healthBarUI = healthBarUI;
        bossHealth.healthBarFill = healthBarFill;

        // Initially hide the health bar
        healthBarUI.SetActive(false);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up all created objects
        if (bossGameObject != null)
            Object.DestroyImmediate(bossGameObject);
        if (healthBarUI != null)
            Object.DestroyImmediate(healthBarUI);
        if (healthBarFill != null && healthBarFill.gameObject != null)
            Object.DestroyImmediate(healthBarFill.gameObject);
    }

    [Test]
    public void Start_ShouldShowHealthBarWhenBossSpawns()
    {
        // Arrange - health bar starts hidden (done in SetUp)
        Assert.IsFalse(healthBarUI.activeSelf, "Health bar should start hidden");

        // Act - enable the GameObject and manually call Start
        bossGameObject.SetActive(true);

        var startMethod = typeof(BossHealth).GetMethod("Start",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        startMethod?.Invoke(bossHealth, null);

        // Assert - health bar should now be visible
        Assert.IsTrue(healthBarUI.activeSelf, "Health bar should be visible when boss spawns");
        Assert.AreEqual(1f, healthBarFill.fillAmount, "Health bar should show full health (100%)");
    }
}