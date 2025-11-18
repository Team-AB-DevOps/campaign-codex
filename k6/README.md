# K6 Performance Testing

This directory contains performance and load testing scripts for the Campaign Codex application deployed on Hetzner using [k6](https://k6.io/).

## 📋 Prerequisites

-   [k6](https://k6.io/docs/get-started/installation/) installed on your machine
-   Node.js and npm (for TypeScript compilation)

## 🚀 Quick Start

### Install Dependencies

```bash
npm install
```

### Run Tests

```bash
# Stress Test
K6_WEB_DASHBOARD=true k6 run src/stress-test.ts

# Load Test
K6_WEB_DASHBOARD=true k6 run src/load-test.ts

# Spike Test
K6_WEB_DASHBOARD=true k6 run src/spike-test.ts
```

## 📊 Test Types

### 1. Stress Test (`stress-test.ts`)

**Purpose:** Determines the system's breaking point by gradually increasing load until failures occur. Identified maximum capacity at ~1800 concurrent users

**Configuration:**

-   Progressive load increase from 200 to 3000 virtual users
-   10-second stages for each load level
-   Threshold: Request failure rate < 0.01%

**Use Case:** Find the absolute maximum load your system can handle before degradation.


### 2. Load Test (`load-test.ts`)

**Purpose:** Tests system behavior under expected production load over an extended period.

**Configuration:**

-   Ramp up from 200 to 1000 virtual users (55% of max capacity)
-   Sustained load for 60 seconds
-   Gradual ramp down
-   Threshold: 95th percentile response time < 800ms

**Use Case:** Validate performance under normal operating conditions.


### 3. Spike Test (`spike-test.ts`)

**Purpose:** Tests system resilience to sudden traffic spikes.

**Configuration:**

-   Baseline: 30 virtual users
-   Sudden spike to 1400 users (77% of max capacity)
-   Quick recovery to baseline
-   Thresholds:
    -   95th percentile response time < 800ms
    -   Request failure rate < 0.00001%

**Use Case:** Simulate sudden traffic surges (e.g., viral content, flash sales).


## 📈 Understanding Results

### Key Metrics

-   **http_req_duration**: Response time (p95, p99)
-   **http_req_failed**: Failed request rate
-   **http_reqs**: Total requests per second
-   **vus**: Virtual users (concurrent connections)
-   **iterations**: Completed test iterations
