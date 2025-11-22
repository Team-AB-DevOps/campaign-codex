import { test, expect } from '@playwright/test';
import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

test('test', async ({ page }) => {
    await page.goto('http://localhost:3000/');
    await page.getByRole('button', { name: 'To site' }).click();
    await page.getByRole('textbox', { name: 'Email' }).click();
    await page.getByRole('textbox', { name: 'Email' }).fill('brian@test.com');
    await page.getByRole('textbox', { name: 'Password' }).click();
    await page.getByRole('textbox', { name: 'Password' }).fill('Pa$$w0rd');
    await page.getByRole('button', { name: 'Sign in' }).click();
    await page.getByRole('main').locator('svg').click();
    await page.getByRole('textbox', { name: 'Campaign name' }).click();
    await page
        .getByRole('textbox', { name: 'Campaign name' })
        .fill('Test Campaign');
    await page.getByRole('button', { name: 'Create Campaign' }).click();
    await page
        .getByRole('link', { name: 'Delete campaign Test Campaign' })
        .click();
    await page.getByRole('button', { name: 'Add map image' }).click();
    // await page.getByRole('img').first().click();

    // Upload the file to the dropzone file input
    const fileInput = page.locator('input[type="file"]');
    await fileInput.setInputFiles(
        path.join(__dirname, 'assets', 'fantasy_map.webp'),
    );

    // Wait for the "Add photo" button to appear (indicates file was loaded and cropped)
    await page.waitForSelector('button:has-text("Add photo")', {
        state: 'visible',
    });
    await page.getByRole('button', { name: 'Add photo' }).click();
    await page.getByRole('button', { name: 'Close' }).click();

    // Wait for the image upload to complete and the map to load
    await expect(page.locator('#root')).toContainText('Double Click: Add pin', {
        timeout: 10000,
    });
});
