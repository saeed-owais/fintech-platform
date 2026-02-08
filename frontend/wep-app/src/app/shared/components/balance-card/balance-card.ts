import { Component, input } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';

@Component({
    selector: 'app-balance-card',
    imports: [DatePipe, DecimalPipe],
    templateUrl: './balance-card.html',
})
export class BalanceCard {
    balance = input.required<number>();
    lastUpdated = input<string | null>(null);
    isFrozen = input(false);
    variant = input<'default' | 'compact'>('default');
}
