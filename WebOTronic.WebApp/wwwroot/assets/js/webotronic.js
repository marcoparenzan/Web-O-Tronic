var angles = {
    Z: 0,
    P45: Math.PI / 4,
    P90: Math.PI / 2,
    P135: 3 * Math.PI / 4,
    P180: Math.PI,
    M180: -Math.PI,
    M135: -3 * Math.PI / 4,
    M90: -Math.PI / 2,
    M45: -Math.PI / 4,
};

var world = undefined;

var World = function (options) {

    var screen_hw = (kontra.canvas.height / kontra.canvas.width);

    this.x1 = options.x1 || 0;
    this.y1 = options.y1 || 0;
    this.x2 = options.x2 || 1;
    this.y2 = options.y2 || screen_hw;

    this._width = this.x2 - this.x1;
    this._height = this.y2 - this.y1;
    var current_hw = this._height / this._width;

    var diff = current_hw < screen_hw;

    // NOW TRY TO CORRECT
    if (diff<0) {
        if (screen_hw < 1) { // vertical
            // add to x
            var delta = (diff * (this.x2 - this.x1)) / 2;
            this.x2 += delta;
            this.x1 -= delta;
        }
        else { // horizontal
            // add to y
            var delta = (diff * (this.y2 - this.y1))/2;
            this.y2 += delta;
            this.y1 -= delta;
        }
    }
    else if (diff>0) {
        if (screen_hw < 1) { // vertical
            // add to y
            var delta = (diff * (this.y2 - this.y1)) / 2;
            this.y2 += delta;
            this.y1 -= delta;
        }
        else { // horizontal
            // add to x
            var delta = (diff * (this.x2 - this.x1)) / 2;
            this.x2 += delta;
            this.x1 -= delta;
        }
    }
    // else perfect

    //

    this._scalex_p2w = this._width / kontra.canvas.width;
    this._scalex_w2p = kontra.canvas.width / this._width;
    this._scaley_p2w = this._height / kontra.canvas.height;
    this._scaley_w2p = kontra.canvas.height / this._height;

    this.rescalex = function (x) {
        return x * kontra.canvas.width / 1300;
    };

    this.rescaley = function (y) {
        return y * kontra.canvas.height / 850;
    };

    this.sizefrompixel = function (w, h) {
        var self = this;
        return {
            width: w * self._scalex_p2w,
            height: h * self._scaley_p2w,
        };
    };

    this.sizefromworld = function (w, h) {
        var self = this;
        return {
            width: w * self._scalex_w2p,
            height: h * self._scaley_w2p,
        };
    };

    this.frompixel = function (s) {
        var self = this;
        return s * self._scalex_p2w;
    };

    this.fromworld = function (s) {
        var self = this;
        return s * self._scalex_w2p;
    };

    this.xfrompixel = function (x) {
        var self = this;
        return x * self._scalex_p2w;
    };

    this.xfromworld = function (x) {
        var self = this;
        return x * self._scalex_w2p;
    };

    this.yfrompixel = function (y) {
        var self = this;
        return y * self._scaley_p2w;
    };

    this.yfromworld = function (y) {
        var self = this;
        return y * self._scaley_w2p;
    };

    this.sprite = kontra.sprite({
        render: function () {
            var self = this;
            var ctx = self.context;
            ctx.strokeStyle = '#33CCFF';
            ctx.lineWidth = world.rescalex(5);
            ctx.strokeWidth = world.rescalex(5);

            ctx.beginPath();
            ctx.moveTo(world.rescalex(5), world.rescaley(5));
            ctx.lineTo(kontra.canvas.width - world.rescalex(5), world.rescaley(5));
            ctx.closePath();
            ctx.stroke();

            ctx.beginPath();
            ctx.moveTo(world.rescalex(5), kontra.canvas.height - world.rescaley(5));
            ctx.lineTo(kontra.canvas.width - world.rescalex(5), kontra.canvas.height - world.rescaley(5));
            ctx.closePath();
            ctx.stroke();

            ctx.beginPath();
            ctx.moveTo(kontra.canvas.width / 2, world.rescaley(5));
            ctx.lineTo(kontra.canvas.width / 2, kontra.canvas.height - world.rescaley(5));
            ctx.closePath();
            ctx.stroke();

            ctx.font = parseInt(world.rescalex(64)) + "px pong";
            ctx.fillStyle = "#33CCFF";
            ctx.textAlign = "center";
            ctx.fillText(options.scoreText(), kontra.canvas.width / 2, world.rescaley(100));
        }
    });

    this.render = function () {
        var self = this;
        self.sprite.render();
    };

    this.update = function () {
        var self = this;
    };

    return this
};

var XYVector = function (x, y) {

    this.clone = function () {
        var self = this;
        return new XYVector(self.x, self.y);
    };

    this.add = function (v) {
        var self = this;
        self.x += v.x;
        self.y += v.y;
        return this;
    };

    this.x = x;
    this.y = y;

    return this;
};

var PolarVector = function (mag, angle) {

    this.clone = function () {
        var self = this;
        return new PolarVector(self.mag, self.angle);
    };

    this.updateangle = function (angle) {
        var self = this;
        self.angle = angle;
        self.x = self.mag * Math.cos(self.angle);
        self.y = self.mag * Math.sin(self.angle);
        return this;
    };

    this.updatey = function (y) {
        var self = this;
        self.y = y;
        self.mag = Math.sqrt(self.x * self.x + self.y * self.y);
        self.angle = Math.atan2(self.y, self.x);
        return this;
    }

    this.updatex = function (y) {
        var self = this;
        self.x = x;
        self.mag = Math.sqrt(self.x * self.x + self.y * self.y);
        self.angle = Math.atan2(self.y, self.x);
        return this;
    }

    this.mag = mag || 1;
    this.updateangle(angle || 0);

    return this;
};

var Ball = function (options) {

    this.boundary = options.boundary;

    this.movement = world.frompixel(options.movement || 8);

    this.width = options.width || world.xfrompixel(24);
    this.height = options.height || world.yfrompixel(24);

    this.state = "stopped";

    this.position = options.position || new XYVector(
        ((this.boundary.x2 - this.boundary.x1) / 2) - (this.width / 2),
        ((this.boundary.y2 - this.boundary.y1) / 2) - (this.height / 2)
    );
    if (Math.random() > 0.5) {
        this.speed = new PolarVector(this.movement, angles.M135);
    }
    else {
        this.speed = new PolarVector(this.movement, angles.P135);
    }

    this.sprite = kontra.sprite({
        width: this.width,
        height: this.height,
        color: options.color || 'yellow',
        image: options.imageName !== undefined ? kontra.assets.images["/assets/sprites/" + options.imageName] : undefined
    });

    this.bouncex = function (paddle) {
        var self = this;

        var index = (self.position.y - paddle.position.y) / (paddle.height / 2);
        if (index < -0.98) index = -0.98;
        if (index > +0.98) index = +0.98;

        var alpha = 0;
        if (self.speed.x < 0) { // left
            alpha = index * Math.PI / 3;
        }
        else if (self.speed.x > 0) { // right
            alpha = Math.PI - index * Math.PI / 3;
        }
        self.speed.updateangle(alpha);
        return this;
    };

    this.bouncey = function () {
        var self = this;
        self.speed.updatey(-self.speed.y);
        return this;
    };

    this.run = function () {
        var self = this;
        self.state = "running";
    };

    this.score = function (paddle, other) {
        var self = this;

        paddle.wonpoint();
        other.lostpoint();

        self.state = "stopped";

        self.position.x = other.position.x;
        self.position.y = other.position.y;
        if (other.position.x > kontra.canvas.width / 2) {
            self.speed = new PolarVector(this.movement, angles.P135);
        }
        else {
            self.speed = new PolarVector(this.movement, angles.M45);
        }
    };

    this.update = function () {
        var self = this;
        switch (self.state) {
            case "stopped":
                break;
            case "running":
                //if (self.speed !== undefined) {
                //    self.position.add(self.speed);
                //}
                break;
        }
    };

    this.render = function () {
        var self = this;
        switch (self.state) {
            case "stopped":
                break;
            case "running":
                self.sprite.x = world.xfromworld(self.position.x - self.width / 2);
                self.sprite.y = world.yfromworld(self.position.y - self.height / 2);
                self.sprite.render();
                break;
        }
    };

    return this;
};

var Paddle = function (options) {

    this.boundary = options.boundary;

    this.score = options.score || 0;
    this.restartposition = options.restartposition;
    this.movement = world.yfrompixel(options.movement || 12);

    this.position = this.restartposition.clone();
    this.speed = options.speed; // speed is a magnitude

    this.width = options.width || world.xfrompixel(50);
    this.height = options.height || world.yfrompixel(160);

    this.sprite = kontra.sprite({
        width: world.xfromworld(this.width),
        height: world.yfromworld(this.height),
        color: options.color || "white",
        image2: options.imageName !== undefined ? kontra.assets.images["/assets/sprites/" + options.imageName] : undefined,
        render: function () {
            var self = this;
            self.context.drawImage(self.image2, self.x, self.y, self.width, self.height);
        }
    });

    this.up = function () {
        var self = this;
        if (self.position.y > self.boundary.y1) {
            self.position.y -= self.movement;
        }
    };
    this.down = function () {
        var self = this;
        if (self.position.y < self.boundary.y2) {
            self.position.y += self.movement;
        }
    };
    this.left = function () {
        var self = this;
        if (self.position.x > self.boundary.x1) {
            self.position.x -= self.movement;
        }
    };
    this.right = function () {
        var self = this;
        if (self.position.x < self.boundary.x2) {
            self.position.x += self.movement;
        }
    };

    this.wonpoint = function () {
        var self = this;
        self.score++;
        self.position = self.restartposition.clone();
    };

    this.lostpoint = function () {
        var self = this;
        self.position = self.restartposition.clone();
    };

    this.update = function () {
        var self = this;
        if (self.speed !== undefined) {
            self.position.add(self.speed);
        }
    };

    this.render = function () {
        var self = this;
        self.sprite.x = world.xfromworld(self.position.x - self.width / 2);
        self.sprite.y = world.yfromworld(self.position.y - self.height / 2);
        self.sprite.render();
    };

    return this;
};
