import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'oidc';

  constructor(
    public oidcSecurityService: OidcSecurityService,
    public http: HttpClient
  ) {}

  ngOnInit(): void {
    this.oidcSecurityService
      .checkAuth()
      .subscribe(({ isAuthenticated, userData }) => {
        console.log('is authenticated', isAuthenticated);
        console.log('User Data', userData);
      });
  }

  login(): void {
    this.oidcSecurityService.authorize();
  }

  callApiOne(): void {
    const accessToken = this.oidcSecurityService.getAccessToken();

    this.http
      .get('https://localhost:5001/secret', {
        headers: new HttpHeaders({
          Authorization: `Bearer ${accessToken}`,
        }),
        responseType: 'text',
      })
      .subscribe((data: any) => {
        console.log('api result', data);
      });
  }

  logout(): void {
    this.oidcSecurityService.logoff();
  }
}
