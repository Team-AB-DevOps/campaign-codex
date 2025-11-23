import { test, expect } from '@playwright/test';
import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

test('should complete full dungeon master workflow from login, create campaign, upload map, add player, add wiki entry, edit notes', async ({
    page,
}) => {
    await page.goto('http://localhost:3000/');
    await page.getByRole('button', { name: 'To site' }).click();
    await page.getByRole('textbox', { name: 'Email' }).click();
    await page.getByRole('textbox', { name: 'Email' }).fill('brian@test.com');
    await page.getByRole('textbox', { name: 'Password' }).click();
    await page.getByRole('textbox', { name: 'Password' }).fill('Pa$$w0rd');
    await page.getByRole('button', { name: 'Sign in' }).click();
    await page.locator('.lucide-diamond-plus').click();
    await page.getByRole('textbox', { name: 'Campaign name' }).click();
    await page
        .getByRole('textbox', { name: 'Campaign name' })
        .fill('Test Campaign');
    await page.getByRole('button', { name: 'Create Campaign' }).click();
    await page
        .getByRole('link', { name: 'Delete campaign Test Campaign' })
        .click();
    await page.getByRole('button', { name: 'Add map image' }).click();

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
    await page.getByRole('link', { name: 'Players' }).click();
    await expect(page.locator('#root')).toContainText(
        'Currently no players in this campaign.',
    );
    await page.getByRole('button', { name: 'Add player' }).click();
    await page.getByRole('textbox', { name: 'Player email' }).click();
    await page
        .getByRole('textbox', { name: 'Player email' })
        .fill('frederik@test.com');
    await page.getByRole('button', { name: 'Add Player' }).click();
    await expect(page.getByText('Added player to campaign')).toBeVisible();
    await page.getByRole('link', { name: 'Wiki' }).click();
    await page.getByRole('button', { name: 'Create New Entry' }).click();
    await page.getByRole('textbox', { name: 'Name' }).click();
    await page
        .getByRole('textbox', { name: 'Name' })
        .fill('Castle Brightstorm');

    await page.getByRole('combobox', { name: 'Type' }).click();
    await page.getByRole('option', { name: 'Location' }).click();
    await page.getByRole('checkbox', { name: 'Is Visible to Players' }).click();
    await page.getByRole('paragraph').click();
    await page
        .locator('.tiptap')
        .fill("Castle Brightstorm is a big ol' castle!");
    await page.getByText('Castle Brightstorm is a big').dblclick();
    await page.locator('button:nth-child(4)').click();
    await page.getByRole('textbox', { name: 'Name' }).click();
    await page
        .getByRole('textbox', { name: 'Name' })
        .fill('Castle Brightstorm');
    await page.getByRole('button', { name: 'Create Entry' }).click();
    await expect(
        page.getByText('Wiki entry created successfully'),
    ).toBeVisible();

    await page.getByRole('link', { name: 'Notes' }).click();
    await page.locator('.lucide.lucide-pencil').click();

    await page.getByText('Type your own notes here!').click();
    await page
        .getByText('Type your own notes here!')
        .fill('These are my own notes! 😎');
    await page.getByRole('button', { name: 'Save' }).click();
    await expect(page.getByText('Saved notes successfully!')).toBeVisible();

    await page
        .getByRole('link', { name: 'Dungeon and Dragons Campaign' })
        .click();

    // Find the campaign card and click the delete icon (first match)
    const campaignCard = page.locator('a:has-text("Test Campaign")').first();
    await campaignCard.locator('[aria-label="Delete campaign"]').click();
    await expect(page.getByText('Delete Campaign?')).toBeVisible();
    await page.getByRole('button', { name: 'Confirm' }).click();
    await expect(page.getByText('Deleted campaign!')).toBeVisible();
});
