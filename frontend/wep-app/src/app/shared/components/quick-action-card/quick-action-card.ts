import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-quick-action-card',
    imports: [RouterLink],
    templateUrl: './quick-action-card.html'
})
export class QuickActionCard {
    title = input.required<string>();
    route = input.required<string>();
    icon = input.required<'deposit' | 'withdraw' | 'transfer'>();
    color = input<'green' | 'red' | 'blue'>('green');
}
