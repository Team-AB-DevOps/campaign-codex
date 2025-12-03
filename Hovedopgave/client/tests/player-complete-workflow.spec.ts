import { test, expect } from '@playwright/test';

test('should complete full player workflow from login, create character, find wiki entry, logout', async ({
    page,
}) => {
    // Login
    await page.goto('/');
    await page.getByRole('button', { name: 'To site' }).click();
    await page.getByRole('textbox', { name: 'Email' }).click();
    await page.getByRole('textbox', { name: 'Email' }).fill('brian@test.com');
    await page.getByRole('textbox', { name: 'Password' }).click();
    await page.getByRole('textbox', { name: 'Password' }).fill('Pa$$w0rd');
    await page.getByRole('button', { name: 'Sign in' }).click();

    // Create character
    await page.getByRole('link', { name: 'Storm Watch DM: Frederik' }).click();
    await expect(page.getByRole('navigation')).toContainText('Player');
    await page.getByRole('link', { name: 'Players' }).click();
    await page.getByRole('button', { name: 'Create Character' }).click();
    await page.getByRole('textbox', { name: 'Name' }).fill('Gimli');
    await page.getByRole('combobox', { name: 'Race' }).click();
    await page.getByRole('option', { name: 'Dwarf' }).click();
    await page.getByRole('combobox', { name: 'Class' }).click();
    await page.getByRole('option', { name: 'Fighter' }).click();
    await page.locator('.tiptap').fill("This is Gimli's backstory.");
    await page
        .locator('div')
        .filter({ hasText: /^Create Character$/ })
        .click();
    await expect(
        page.getByText('Character created successfully'),
    ).toBeVisible();

    // Retire character
    await page.waitForLoadState('networkidle');
    await page.locator('.lucide.lucide-pencil').click();
    await page.getByRole('button', { name: 'Retire character' }).click();
    await expect(page.getByText('Retire Character?')).toBeVisible();

    await page.getByRole('button', { name: 'Confirm' }).click();
    await expect(
        page.getByText('Retired character:', { exact: false }),
    ).toBeVisible();

    // Find wiki entry
    await page.getByRole('link', { name: 'Wiki' }).click();
    await page.getByRole('textbox', { name: 'Search...' }).fill('Spire');
    await page.getByRole('button', { name: 'Locations' }).click();
    await page.getByText('The Shattered Spire').click();
    await expect(
        page.getByText(
            'A towering crystalline structure that pierces the sky, broken at its peak during the Cataclysm.',
        ),
    ).toBeVisible();

    // Log out
    await page.getByRole('button').nth(1).click();
    await page.getByRole('menuitem', { name: 'Logout' }).click();
    await page.goto('/login');
    await expect(page.getByRole('heading')).toContainText('Sign in');
});
