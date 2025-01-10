import http from 'k6/http';

export const options = {
  stages: [
    { duration: '10s', target: 100 },
    { duration: '40s', target: 100 },
  ]
};

export default function() {
  http.get('http://localhost:8080/dapper/vehicles/simple?page=0&pageSize=30');
}
