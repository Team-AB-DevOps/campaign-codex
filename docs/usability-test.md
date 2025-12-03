# Campaign Codex - Usability Test Plan

**Date:** December 3, 2025  
**Version:** 1.0  
**Product:** Campaign Codex - Web-based Tabletop RPG Campaign Management Application

---

## Target User Group

- **Age Range:** 16-45 years
- **Technical Proficiency:** Basic web navigation skills, familiar with social media or gaming platforms
- **Current Practices:** Using character sheets (physical or digital), taking personal notes

### User Characteristics

- **Gaming Frequency:** Play or run games at least twice per month
- **Digital Literacy:** Comfortable with web browsers, file uploads, and basic text editing
- **Motivation:** Seeking better organization, collaboration, and accessibility of campaign materials
- **Device Usage:** Primarily desktop/laptop users, with potential mobile access

---

## Measures

### Preference Measures (Subjective)

#### 10-question survey with 5-point scale (Strongly Disagree to Strongly Agree)

1. I think that I would like to use this system frequently
2. I found the system unnecessarily complex
3. I thought the system was easy to use
4. I think that I would need the support of a technical person to be able to use this system
5. I found the various functions in this system were well integrated
6. I thought there was too much inconsistency in this system
7. I would imagine that most people would learn to use this system very quickly
8. I found the system very cumbersome to use
9. I felt very confident using the system
10. I needed to learn a lot of things before I could get going with this system

#### Overall Desirability (Yes/No Questions) - Post-Test

1. Would you recommend Campaign Codex to other TTRPG players/DMs? (Yes/No)
2. Would you consider using Campaign Codex for your own campaigns? (Yes/No)
3. Does Campaign Codex solve problems you currently face in campaign management? (Yes/No)
4. Would you choose Campaign Codex over your current method of organization? (Yes/No)


### Performance Measures (Objective)

#### Time on Task

- **Measurement:** Record time from task initiation to successful completion
- **Success Threshold:**
  - Simple tasks (e.g., navigation): ≤ 30 seconds
  - Medium tasks (e.g., creating a campaign): ≤ 2 minutes
  - Complex tasks (e.g., uploading map with pins): ≤ 4 minutes

#### Task Completion Rate

- **Measurement:** Percentage of tasks completed successfully without assistance
- **Success Criteria:**
  - Completed without help: Full success
  - Completed with minimal hint: Partial success
  - Unable to complete or required significant help: Failure

#### Error Count

**Error Types:**

1. **Navigation Errors:** Clicking wrong menu items, getting lost
2. **Data Entry Errors:** Incorrect form filling, validation errors
3. **Feature Misuse:** Using features incorrectly or in unintended ways
4. **Recovery Failures:** Unable to undo or correct mistakes

**Measurement:** Count of each error type per task

#### Help Requests

- **Measurement:** Number of times participant asks for clarification or assistance

---

## Test Script

#### **Scenario 1: Dungeon Master - Getting Started**

**Role:** Dungeon Master  
**Context:** "You are a Dungeon Master starting a new D&D campaign with your friends. You've heard about Campaign Codex and want to try using it to organize your campaign materials."

##### Task 1.1: Account Registration

**Instructions:**  
"Please create an account on Campaign Codex using the email and password of your choice."

**Success Criteria:**

- Successfully navigates to registration page
- Fills out all required fields correctly
- Completes registration and lands on campaigns page

**Expected Time:** 1-2 minutes  
**Common Errors:** Password requirements confusion, email validation issues

---

##### Task 1.2: Create Your First Campaign

**Instructions:**  
"Create a new campaign called 'The Lost Mines' that you'll use for your upcoming adventure."

**Success Criteria:**

- Locates "Create Campaign" button/option
- Enters campaign name
- Successfully creates campaign
- Navigates to campaign dashboard

**Expected Time:** 30-45 seconds  
**Common Errors:** Confusion about where to click, missing confirmation

---

#### **Scenario 2: Dungeon Master - Map Management**

**Role:** Dungeon Master  
**Context:** "Your campaign takes place in a region called Silverwood Valley. You have a map image that you want to upload and mark important locations."

##### Task 2.1: Upload Campaign Map

**Instructions:**  
"Upload the map image I'm providing to you for your campaign."

**Provided:** Test map image file (silverwood_valley_map.jpg)

**Success Criteria:**

- Navigates to Map section
- Finds upload functionality
- Successfully uploads image
- Confirms map is visible

**Expected Time:** 1-2 minutes  
**Common Errors:** Can't find upload button, image preview issues

---

##### Task 2.2: Add Location Pins to Map

**Instructions:**  
"Add three pins to your map for these locations:

1. A town called 'Millhaven' in the center area
2. A dungeon called 'Dark Caverns' in the northwest
3. A forest called 'Whispering Woods' in the southeast

Add a brief description to each pin."

**Success Criteria:**

- Adds all three pins to map
- Positions pins correctly (approximate)
- Enters names and descriptions for each
- Saves pins successfully

**Expected Time:** 3-4 minutes  
**Common Errors:** Pin placement difficulties, forgetting to save, unclear icons

---

#### **Scenario 3: Dungeon Master - Wiki Entry Creation**

**Role:** Dungeon Master  
**Context:** "You want to document an important NPC that the players will meet. You'll create a wiki entry for this character."

##### Task 3.1: Create NPC Wiki Entry

**Instructions:**  
"Create a wiki entry for an NPC named 'Elara Moonwhisper,' an elf merchant. Include the following information:

- Type: NPC
- Description: 'A mysterious elven merchant who trades in rare magical items'
- Make it visible to players
- Upload the character portrait image I'm providing"

**Provided:** Character portrait image (elara_portrait.jpg)

**Success Criteria:**

- Navigates to Wiki section
- Creates new wiki entry
- Selects NPC type
- Enters name and description
- Uploads image
- Sets visibility to visible
- Saves entry successfully

**Expected Time:** 2-3 minutes  
**Common Errors:** Visibility toggle confusion, image upload issues, rich text editor confusion

---

#### **Scenario 4: Dungeon Master - Player Management**

**Role:** Dungeon Master  
**Context:** "Your friend Sarah wants to join your campaign. You need to add her as a player."

##### Task 4.1: Add Player to Campaign

**Instructions:**  
"Add a player to your campaign using this email: <sarah.player@test.com>"

**Success Criteria:**

- Navigates to Players section
- Finds "Add Player" functionality
- Enters player email
- Successfully sends invitation/adds player
- Confirms player is in the list

**Expected Time:** 1 minute  
**Common Errors:** Can't find add player button, unclear confirmation

---

#### **Scenario 5: Player - Joining and Exploring**

**Role:** Player  
**Context:** "You're a player who has been invited to join 'The Lost Mines' campaign. You'll log in and explore what your DM has created."

**Note:** Switch to pre-created player account for this scenario

##### Task 5.1: Navigate Campaign Content

**Instructions:**  
"You've just logged in. Take a moment to:

1. View the campaign map
2. Find the wiki entry for 'Elara Moonwhisper'
3. Check what other players are in the campaign"

**Success Criteria:**

- Successfully navigates to Map section and views map
- Locates and opens Wiki section
- Finds NPC entry
- Navigates to Players section and views player list

**Expected Time:** 2-3 minutes  
**Common Errors:** Navigation confusion, missing sidebar items

## Consent Statement

By signing below, I confirm that:

- I have read and understood the information provided above
- I have had the opportunity to ask questions and received satisfactory answers
- I understand that my participation is voluntary and I may withdraw at any time
- I understand that the session will be recorded (screen and audio)
- I consent to participate in this usability study

---

**Participant Name (Printed):** ___________________________________

**Participant Signature:** ___________________________________

**Date:** ___________________________________

---
