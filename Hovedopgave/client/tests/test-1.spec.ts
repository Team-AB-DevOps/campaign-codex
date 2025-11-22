import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
    await page.goto('http://localhost:3000/');
    await expect(page.locator('h1')).toContainText('Campaign Codex');
    await page.getByRole('button', { name: 'To site' }).click();
    await page.getByRole('textbox', { name: 'Email' }).click();
    await page.getByRole('textbox', { name: 'Email' }).fill('brian@test.com');
    await page.getByRole('textbox', { name: 'Password' }).click();
    await page.getByRole('textbox', { name: 'Password' }).fill('Pa$$w0rd');
    await page.getByRole('button', { name: 'Sign in' }).click();
    await expect(page.getByRole('main')).toContainText('Hello Brian');
    await page.getByRole('img').nth(3).click();
    await expect(page.getByText('Create new campaign')).toBeVisible();
    await page.getByRole('textbox', { name: 'Campaign name' }).click();
    await page
        .getByRole('textbox', { name: 'Campaign name' })
        .fill('Test Campaign');
    await page.getByRole('button', { name: 'Create Campaign' }).click();
    await expect(page.getByRole('main')).toContainText('Test Campaign');

    await page.getByRole('link', { name: 'Test Campaign' }).click();
    
    await expect(page.locator('#root')).toContainText('No map image available');
   await page.getByRole('button', { name: 'Add map image' }).click();
 
   

    // Find the campaign card and click the delete icon (first match)
    const campaignCard = page.locator('a:has-text("Test Campaign")').first();
    await campaignCard.locator('[aria-label="Delete campaign"]').click();
    await expect(page.getByText('Delete Campaign?')).toBeVisible();
    await page.getByRole('button', { name: 'Confirm' }).click();
    await expect(page.getByText('Deleted campaign!')).toBeVisible();
});
