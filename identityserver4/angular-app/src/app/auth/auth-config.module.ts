import { NgModule } from '@angular/core';
import { AuthModule } from 'angular-auth-oidc-client';

@NgModule({
    imports: [
        AuthModule.forRoot({
        config: {
            authority: 'https://localhost:5000',
            redirectUrl: window.location.origin,
            postLogoutRedirectUri: window.location.origin,
            clientId: 'angular',
            scope: 'openid ApiOne.user', // 'openid profile ' + your scopes
            responseType: 'code',
            silentRenew: false,
            renewTimeBeforeTokenExpiresInSeconds: 10,
        },
        }),
    ],
    exports: [AuthModule],
})
export class AuthConfigModule {}
