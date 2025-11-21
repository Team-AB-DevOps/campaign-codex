// import necessary modules
import { check } from "k6";
import http from "k6/http";

// define configuration
export const options = {
    // define thresholds
    thresholds: {
        http_req_duration: [{ threshold: "p(95)<800", abortOnFail: true, delayAbortEval: "10s" }], // Latency threshold for percentile
        http_req_failed: [
            { threshold: "rate<0.0000001", abortOnFail: true, delayAbortEval: "10s" }, // No Requests failing
        ],
    },

    stages: [
        { duration: "10s", target: 30 }, // 0 -> 30
        { duration: "10s", target: 30 }, // 30, 10 sec
        { duration: "10s", target: 1400 }, // Spike to 1400/1800 = 77%
        { duration: "10s", target: 30 }, // 1400 -> 30
        { duration: "10s", target: 0 }, // 30 -> 0

    ],
};

export default function () {
    // define URL and request body
    const res = http.get("http://46.224.39.173");

    // check that response is 200
    check(res, {
        "response code was 200": (res) => res.status == 200,
    });
}
