using NUnit.Framework;
using UnityEngine;

public class PlayerBugShooterTests
{
    private GameObject playerObject;
    private PlayerBugShooter shooter;
    private GameObject mockBugPrefab;

    [SetUp]
    public void SetUp()
    {
        // Create test objects before each test
        playerObject = new GameObject("TestPlayer");
        shooter = playerObject.AddComponent<PlayerBugShooter>();

        // Create a mock bug prefab for testing
        mockBugPrefab = new GameObject("MockBug");
        mockBugPrefab.AddComponent<PlayerBug>();
        shooter.bugPrefab = mockBugPrefab;
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up after each test
        Object.DestroyImmediate(mockBugPrefab);
        Object.DestroyImmediate(playerObject);
    }

    [Test]
    public void ShootBugAtEnemy_WithNullBugPrefab_DoesNotCreateBug()
    {
        // Arrange
        shooter.bugPrefab = null;
        GameObject enemy = new GameObject("TestEnemy");
        int initialChildCount = shooter.transform.childCount;

        // Act
        shooter.ShootBugAtEnemy(enemy.transform);

        // Assert
        Assert.AreEqual(initialChildCount, shooter.transform.childCount,
            "No bug should be created when bugPrefab is null");

        // Cleanup
        Object.DestroyImmediate(enemy);
    }
}