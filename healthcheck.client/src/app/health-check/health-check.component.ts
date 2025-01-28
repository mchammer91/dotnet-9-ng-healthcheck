import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {environment} from "../../environments/environment";

@Component({
  selector: 'app-health-check',
  templateUrl: './health-check.component.html',
  styleUrl: './health-check.component.css'
})
export class HealthCheckComponent implements OnInit {
  public result?: Result;

  constructor(private httpClient: HttpClient) { }

  ngOnInit() {
    this.httpClient.get<Result>(environment.baseUrl + 'api/health').subscribe((result: Result) => {
      this.result = result;
    }, error => console.error(error));
  }
}

interface Result {
  checks: Check[];
  totalStatus: string;
  totalResponseTime: number;
}

interface Check {
  name: string;
  responseTime: number;
  status: string;
  description: string;
}
