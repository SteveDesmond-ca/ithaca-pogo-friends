const app = new Vue({
    el: "#app",
    data: {
        friends: [],
        sort: 'newest',
        search: '',
        filter: {
            raids: false,
            friendship: false,
            pvp: false,
            trading: false
        }
    },
    computed: {
        sorted_and_filtered: function () {
            const search_results = this.friends.filter(friend =>
                friend.name.toLowerCase().indexOf(this.search.toLowerCase()) !== -1
                || friend.username.toLowerCase().indexOf(this.search.toLowerCase()) !== -1);
            
                const filtered = search_results.filter(friend => 
                    (!this.filter.raids || friend.raids)
                    && (!this.filter.friendship || friend.friendship)
                    && (!this.filter.pvp || friend.pvp)
                    && (!this.filter.trading || friend.trading))
            switch (this.sort) {
                case 'newest':
                    return filtered.slice().reverse();
                case 'oldest':
                    return filtered.slice();
                case 'name':
                    return filtered.slice().sort((f1, f2) => f1.name > f2.name ? 1 : -1);
                case 'username':
                    return filtered.slice().sort((f1, f2) => f1.username > f2.username ? 1 : -1);
            }
        }
    },
    mounted: async function () {
        const request = await fetch('friends.json');
        this.friends = await request.json();
        this.friends.forEach(friend => {
            const segments = qrcodegen.QrSegment.makeSegments(friend.code.replace(/ /g, ''));
            friend.qr_code = qrcodegen.QrCode.encodeSegments(segments, qrcodegen.QrCode.Ecc.HIGH, 1, 1, -1, true).toSvgString(1);
        });
    }
});