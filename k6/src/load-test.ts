// import necessary modules
import { check } from "k6";
import http from "k6/http";

// define configuration
export const options = {
    // define thresholds
    thresholds: {
        http_req_duration: [{ threshold: "p(95)<800", abortOnFail: true, delayAbortEval: "10s" }], // Latency threshold for percentile
    },

    stages: [
        { duration: "10s", target: 200 },
        { duration: "10s", target: 400 },
        { duration: "10s", target: 600 },
        { duration: "10s", target: 800 },
        { duration: "60s", target: 1000 }, // 1000 / 1800 = 55% load
        { duration: "10s", target: 800 },
        { duration: "10s", target: 600 },
        { duration: "10s", target: 400 },
        { duration: "10s", target: 200 },
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
