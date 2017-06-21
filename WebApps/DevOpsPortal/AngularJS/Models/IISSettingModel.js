MachineApp.factory('SiteData', function () {
    this.SiteData = function () {

        this.appPoolName = appPoolName;
        this.active = active;
        this.appMessage = message;
        this.physicalPath = physicalPath;
        this.siteId = siteId;
        this.siteName = name;
        this.state = state;
        this.configKeys = configKeys;
        this.bindings = bindings;
        this.keepAlive = setKeepAlive(this.active);
    }

    function setKeepAlive(active) {
        if (this.active)
            return 'OK';
        else if (this.configKeys)
            return 'Unavailable';
        else
            return 'n/a';
    }
    return SiteData;
});