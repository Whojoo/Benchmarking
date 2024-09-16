import http from 'k6/http';

export const options = {
  stages: [
    { duration: '10s', target: 100 },
    { duration: '40s', target: 100 },
  ]
};

export default function() {
  http.get('http://localhost:5029/ef/vehicles?page=3&pageSize=30');
}
