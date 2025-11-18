// import necessary modules
import { check } from "k6";
import http from "k6/http";

// define configuration
export const options = {
    // define thresholds
    thresholds: {
        http_req_failed: [
            { threshold: "rate<0.0001", abortOnFail: true, delayAbortEval: "30s" }, // 00.01%
        ]
    },

    stages: [
        { duration: "10s", target: 200 },
        { duration: "10s", target: 400 },
        { duration: "10s", target: 600 },
        { duration: "10s", target: 800 },
        { duration: "10s", target: 1000 },
        { duration: "10s", target: 1200 },
        { duration: "10s", target: 1400 },
        { duration: "10s", target: 1600 },
        { duration: "10s", target: 1800 }, // MAX LOAD before requests start failing
        { duration: "10s", target: 2000 },
        { duration: "10s", target: 2200 },
        { duration: "10s", target: 2400 },
        { duration: "10s", target: 2600 },
        { duration: "10s", target: 2800 },
        { duration: "10s", target: 3000 },
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
